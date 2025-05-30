package com.example.FitPlanner_server.server.controller;

import com.example.FitPlanner_server.server.DTO.GetWorkoutDTO;
import com.example.FitPlanner_server.server.DTO.WorkoutResponse;
import com.example.FitPlanner_server.server.Model.UserModel;
import com.example.FitPlanner_server.server.Model.UserRepo;
import com.example.FitPlanner_server.server.Model.WorkoutModel;
import com.example.FitPlanner_server.server.Model.WorkoutRepository;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.core.annotation.AuthenticationPrincipal;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.web.bind.annotation.*;

import java.sql.Date;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.time.LocalDate;

@RestController
@PreAuthorize("hasRole('USER')")
public class WorkoutController {
    private UserRepo userRepo;
    private WorkoutRepository workoutRepository;

    public WorkoutController(WorkoutRepository workoutRepository, UserRepo userRepo)
    {
        this.workoutRepository = workoutRepository;
        this.userRepo = userRepo;
    }

    @PutMapping("/workout/save")
    public void saveWorkout(@RequestBody WorkoutModel workoutModel, @AuthenticationPrincipal UserDetails userDetails)
    {
        UserModel user = userRepo.findByUsername(userDetails.getUsername());
        WorkoutModel workout = workoutRepository.findByDate(workoutModel.getDate());
        if(workout != null)
        {
            workoutRepository.update(workoutModel);
        }
        else
        {
            workoutModel.setUserId(user);
            workoutRepository.save(workoutModel);
        }
    }
    @PutMapping("/workout/get")
    public ResponseEntity<?> getWorkoutNotes(@RequestBody GetWorkoutDTO workoutDTO, @AuthenticationPrincipal UserDetails userDetails)
    {
        UserModel user = userRepo.findByUsername(userDetails.getUsername());
        WorkoutResponse workout = workoutRepository.getWorkout(workoutDTO.getDate(), user.getId());
        if(workout == null)
        {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body("Інформації про тренування не знайдено");
        }
        return ResponseEntity.status(HttpStatus.OK).body(workout);
    }
}

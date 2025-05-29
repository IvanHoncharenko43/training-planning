package com.example.FitPlanner_server.server.controller;

import com.example.FitPlanner_server.server.Model.UserModel;
import com.example.FitPlanner_server.server.Model.UserRepo;
import com.example.FitPlanner_server.server.Model.WorkoutModel;
import com.example.FitPlanner_server.server.Model.WorkoutRepository;
import org.apache.catalina.User;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.core.annotation.AuthenticationPrincipal;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

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
        workoutModel.setUserId(user);
        System.out.println(user.getUsername() + " " + workoutModel.getUserId().getId());
        workoutRepository.save(workoutModel);
    }
}

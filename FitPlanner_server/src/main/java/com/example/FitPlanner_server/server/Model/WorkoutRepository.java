package com.example.FitPlanner_server.server.Model;

import com.example.FitPlanner_server.server.DTO.AllWorkoutsRespond;
import com.example.FitPlanner_server.server.DTO.WorkoutResponse;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Repository;

import java.sql.*;
import java.sql.Date;
import java.time.LocalDate;
import java.util.*;

@Repository
public class WorkoutRepository {

    @Value("${spring.datasource.url}")
    private String dbUrl;
    @Value("${spring.datasource.username}")
    private String dbUsername;
    @Value("${spring.datasource.password}")
    private String dbPassword;

    public WorkoutModel findByDate(Date date, int userId) {
        try {
            Connection connection = DriverManager.getConnection(dbUrl, dbUsername, dbPassword);
            String query = "SELECT * FROM workouts WHERE date = ? AND user_id = ?";
            PreparedStatement preparedStatement = connection.prepareStatement(query);
            preparedStatement.setDate(1, date);
            preparedStatement.setInt(2, userId);
            ResultSet resultSet = preparedStatement.executeQuery();
            resultSet.next();
            WorkoutModel workoutModel = new WorkoutModel();
            workoutModel.setWeight(resultSet.getInt("weight"));
            workoutModel.setNotes(resultSet.getString("notes"));
            workoutModel.setHasTrained(resultSet.getBoolean("has_trained"));
            workoutModel.setDate(date);

            resultSet.close();
            preparedStatement.close();
            connection.close();
            return workoutModel;
        } catch (SQLException e) {
            System.out.println("saveWorkoutNotesEXC: " + e.getMessage());
        }
        return null;
    }
    public void update(WorkoutModel workoutModel)
    {
        try
        {
            Connection connection = DriverManager.getConnection(dbUrl, dbUsername, dbPassword);
            String request = "UPDATE workouts SET weight = ?, notes = ?, has_trained = ? WHERE date = ?";
            PreparedStatement statement = connection.prepareStatement(request);
            statement.setInt(1, workoutModel.getWeight());
            statement.setString(2, workoutModel.getNotes());
            statement.setBoolean(3, workoutModel.getHasTrained());
            statement.setDate(4, workoutModel.getDate());
            statement.executeUpdate();

            statement.close();
            connection.close();
        }
        catch (SQLException e)
        {
            System.out.println("SQLException UpdateWorkout1: " + e.getMessage());
        }
    }
    public void save(WorkoutModel workoutModel)
    {
        try
        {
            Connection connection = DriverManager.getConnection(dbUrl, dbUsername, dbPassword);
            String query = "INSERT INTO workouts (weight, notes, has_trained, date, user_id) VALUES (?, ?, ?, ?, ?)";
            PreparedStatement preparedStatement = connection.prepareStatement(query);
            preparedStatement.setInt(1, workoutModel.getWeight());
            preparedStatement.setString(2, workoutModel.getNotes());
            preparedStatement.setBoolean(3, workoutModel.getHasTrained());
            preparedStatement.setDate(4, workoutModel.getDate());
            preparedStatement.setInt(5, workoutModel.getUserId().getId());
            preparedStatement.executeUpdate();

            preparedStatement.close();
            connection.close();
        }
        catch (SQLException e)
        {
            System.out.println("saveWorkoutNotesEXC: " + e.getMessage());
        }
    }

    public WorkoutResponse getWorkout(Date date, int user_id)
    {
        try
        {
            Connection connection = DriverManager.getConnection(dbUrl, dbUsername, dbPassword);
            String query = "SELECT * FROM workouts WHERE date = ? AND user_id = ?";
            PreparedStatement preparedStatement = connection.prepareStatement(query);
//            LocalDate localDate = date.toLocalDate();
//            Timestamp start = Timestamp.valueOf(localDate.atStartOfDay());
//            Timestamp end = Timestamp.valueOf(localDate.plusDays(1).atStartOfDay());
            preparedStatement.setObject(1, date);
//            preparedStatement.setObject(2, end);
            preparedStatement.setInt(2, user_id);
            ResultSet resultSet = preparedStatement.executeQuery();

            resultSet.next();
            int weight = resultSet.getInt("weight");
            String notes = resultSet.getString("notes");
            boolean hasTrained = resultSet.getBoolean("has_trained");
            WorkoutResponse workout = new WorkoutResponse(weight, notes, hasTrained, date);

            resultSet.close();
            preparedStatement.close();
            connection.close();
            return workout;
        }
        catch (SQLException e)
        {
            System.out.println("getWorkoutNotesEXC: " + e.getMessage());
        }
        return null;
    }

    public List<AllWorkoutsRespond> getAllWorkouts(int userId)
    {
        List<AllWorkoutsRespond> workouts = new ArrayList<>();
        try
        {
            Connection connection = DriverManager.getConnection(dbUrl, dbUsername, dbPassword);
            String query = "SELECT date, weight FROM workouts WHERE user_id = ?";
            PreparedStatement preparedStatement = connection.prepareStatement(query);
            preparedStatement.setInt(1, userId);
            ResultSet resultSet = preparedStatement.executeQuery();
            while(resultSet.next())
            {
                int weight = resultSet.getInt("weight");
                Date date = resultSet.getDate("date");
                workouts.add(new AllWorkoutsRespond(weight, date));
            }
            resultSet.close();
            preparedStatement.close();
            connection.close();
        }
        catch(SQLException e)
        {
            System.out.println("getAllWorkoutsEXC: " + e.getMessage());
        }
        return workouts;
    }
}

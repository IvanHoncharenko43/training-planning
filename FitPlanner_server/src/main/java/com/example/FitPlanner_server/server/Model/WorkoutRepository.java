package com.example.FitPlanner_server.server.Model;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Repository;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.SQLException;

@Repository
public class WorkoutRepository {

    @Value("${spring.datasource.url}")
    private String dbUrl;
    @Value("${spring.datasource.username}")
    private String dbUsername;
    @Value("${spring.datasource.password}")
    private String dbPassword;

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
            preparedStatement.setObject(5, workoutModel.getUserId().getId());
            preparedStatement.executeUpdate();

            preparedStatement.close();
            connection.close();
        }
        catch (SQLException e)
        {
            System.out.println("saveWorkoutNotesEXC: " + e.getMessage());
        }
    }
}

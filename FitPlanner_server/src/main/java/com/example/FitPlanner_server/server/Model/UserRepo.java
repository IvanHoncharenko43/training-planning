package com.example.FitPlanner_server.server.Model;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Repository;

import java.sql.*;

@Repository
public class UserRepo {
    @Value("${spring.datasource.url}")
    private String dbUrl;
    @Value("${spring.datasource.username}")
    private String dbUsername;
    @Value("${spring.datasource.password}")
    private String dbPassword;

    public void save(UserModel user)
    {
        try
        {
            Connection connection = DriverManager.getConnection(dbUrl, dbUsername, dbPassword);
            String request = "INSERT INTO users (name, username, password) VALUES (?, ?, ?)";
            PreparedStatement preparedStatement = connection.prepareStatement(request);
            preparedStatement.setString(1, user.getName());
            preparedStatement.setString(2, user.getUsername());
            preparedStatement.setString(3, user.getPassword());
            preparedStatement.executeUpdate();

            preparedStatement.close();
            connection.close();
        }
        catch (SQLException e)
        {
            System.out.println("SQLException UserRepo1: " + e.getMessage());
        }
    }
    public UserModel findByUsername(String username)
    {
        try
        {
            Connection connection = DriverManager.getConnection(dbUrl, dbUsername, dbPassword);
            String request = "SELECT * FROM users WHERE username = ?";
            PreparedStatement preparedStatement = connection.prepareStatement(request);
            preparedStatement.setString(1, username);
            ResultSet resultSet = preparedStatement.executeQuery();
            if(resultSet.next())
            {
                int id = resultSet.getInt("id");
                String existingName = resultSet.getString("name");
                String existingUsername = resultSet.getString("username");
                String existingPassword = resultSet.getString("password");
                return new UserModel(id, existingName, existingUsername, existingPassword);
            }
            resultSet.close();
            preparedStatement.close();
            connection.close();
        } catch (SQLException e)
        {
            System.out.println("SQLException UserRepo1: " + e.getMessage());
        }
        return null;
    }
}

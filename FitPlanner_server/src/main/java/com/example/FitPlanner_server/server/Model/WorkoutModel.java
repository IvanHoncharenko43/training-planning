package com.example.FitPlanner_server.server.Model;

import jakarta.persistence.*;

import java.time.LocalDate;
import java.util.Date;

@Entity
@Table(name = "workouts")
public class WorkoutModel {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    private int weight;
    private String notes;
    private boolean hasTrained;
    private LocalDate date;

    @ManyToOne
    @JoinColumn(name = "user_id", nullable = false)
    private UserModel userId;

    public WorkoutModel() {}

    public WorkoutModel(int weight, String notes, boolean hasTrained)
    {
        this.weight = weight;
        this.notes = notes;
        this.hasTrained = hasTrained;
    }

    public int getId()
    {
        return id;
    }
    public void setId(int id)
    {
        this.id = id;
    }
    public int getWeight()
    {
        return weight;
    }
    public void setWeight(int weight)
    {
        this.weight = weight;
    }
    public String getNotes()
    {
        return notes;
    }
    public void setNotes(String notes)
    {
        this.notes = notes;
    }
    public boolean getHasTrained()
    {
        return hasTrained;
    }
    public void setHasTrained(boolean hasTrained)
    {
        this.hasTrained = hasTrained;
    }
    public UserModel getUserId()
    {
        return userId;
    }
    public void setUserId(UserModel userId)
    {
        this.userId = userId;
    }
    public java.sql.Date getDate()
    {
        return java.sql.Date.valueOf(date);
    }
    public void setDate(java.sql.Date date)
    {
        this.date = date.toLocalDate();
    }
}
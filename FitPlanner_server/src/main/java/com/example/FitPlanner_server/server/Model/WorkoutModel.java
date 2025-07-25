package com.example.FitPlanner_server.server.Model;

import com.fasterxml.jackson.annotation.JsonFormat;
import jakarta.persistence.*;

import java.time.LocalDate;
import java.sql.Date;

@Entity
@Table(name = "workouts")
public class WorkoutModel {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    private int weight;
    private String notes;
    private boolean hasTrained;

    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "yyyy-MM-dd")
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
    public WorkoutModel(int weight, String notes, boolean hasTrained, Date date)
    {
        this.weight = weight;
        this.notes = notes;
        this.hasTrained = hasTrained;
        this.date = date.toLocalDate();
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
    public Date getDate()
    {
        return Date.valueOf(date);
    }
    public void setDate(Date date)
    {
        this.date = date.toLocalDate();
    }
}
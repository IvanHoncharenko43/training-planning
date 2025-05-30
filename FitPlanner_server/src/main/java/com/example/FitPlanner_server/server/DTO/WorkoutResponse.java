package com.example.FitPlanner_server.server.DTO;

import com.example.FitPlanner_server.server.Model.UserModel;
import com.fasterxml.jackson.annotation.JsonFormat;

import java.sql.Date;
import java.time.LocalDate;

public class WorkoutResponse {

    private int weight;
    private String notes;
    private boolean hasTrained;

    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "yyyy-MM-dd")
    private LocalDate date;

    public WorkoutResponse() {}
    public WorkoutResponse(int weight, String notes, boolean hasTrained, Date date)
    {
        this.weight = weight;
        this.notes = notes;
        this.hasTrained = hasTrained;
        this.date = date.toLocalDate();
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
    public Date getDate()
    {
        return Date.valueOf(date);
    }
    public void setDate(Date date)
    {
        this.date = date.toLocalDate();
    }
}

package com.example.FitPlanner_server.server.DTO;

import com.fasterxml.jackson.annotation.JsonFormat;

import java.sql.Date;
import java.time.LocalDate;

public class AllWorkoutsRespond {

    private int weight;

    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "yyyy-MM-dd")
    private LocalDate date;

    public AllWorkoutsRespond() {}
    public AllWorkoutsRespond(int weight, Date date)
    {
        this.weight = weight;
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
    public Date getDate()
    {
        return Date.valueOf(date);
    }
    public void setDate(Date date)
    {
        this.date = date.toLocalDate();
    }
}

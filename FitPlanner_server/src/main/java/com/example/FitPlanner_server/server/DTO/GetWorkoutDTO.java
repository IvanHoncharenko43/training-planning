package com.example.FitPlanner_server.server.DTO;

import com.fasterxml.jackson.annotation.JsonFormat;

import java.sql.Date;
import java.time.LocalDate;

public class GetWorkoutDTO {

    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "yyyy-MM-dd")
    private LocalDate date;

    public Date getDate()
    {
        return Date.valueOf(date);
    }
    public void setDate(Date date)
    {
        this.date = date.toLocalDate();
    }
}

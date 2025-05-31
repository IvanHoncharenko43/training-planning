package com.example.FitPlanner_server.server.controller;

import com.example.FitPlanner_server.server.DTO.LoginDTO;
import com.example.FitPlanner_server.server.DTO.RegisterDTO;
import com.example.FitPlanner_server.server.Model.UserModel;
import com.example.FitPlanner_server.server.Model.UserRepo;
import com.example.FitPlanner_server.server.component.JwtUtil;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class AuthController {
    private JwtUtil jwtUtil;

    @Autowired
    private AuthenticationManager authenticationManager;

    @Autowired
    private PasswordEncoder passwordEncoder;

    private UserRepo userRepo;

    public AuthController(JwtUtil jwtUtil, UserRepo userRepo) {
        this.jwtUtil = jwtUtil;
        this.userRepo = userRepo;
    }

    @PostMapping("/auth/register")
    public ResponseEntity<?> register(@RequestBody RegisterDTO registerUser)
    {
        UserModel user = userRepo.findByUsername(registerUser.getUsername());
        if(user == null)
        {
            UserModel newUser = new UserModel();
            newUser.setName(registerUser.getName());
            newUser.setUsername(registerUser.getUsername());
            newUser.setPassword(passwordEncoder.encode(registerUser.getPassword()));
            userRepo.save(newUser);
            return ResponseEntity.ok("User registered successfully");
        }
        return ResponseEntity.status(HttpStatus.CONFLICT).body("User already exists");
    }

    @PostMapping("/auth/login")
    public ResponseEntity<?> login(@RequestBody LoginDTO loginUser)
    {
        UserDetails userDetails = null;
        try
        {
            Authentication auth = authenticationManager.authenticate
                    (
                            new UsernamePasswordAuthenticationToken
                                    (
                                            loginUser.getUsername(), loginUser.getPassword()
                                    )
                    );
            userDetails = (UserDetails) auth.getPrincipal();

        }
        catch(Exception e)
        {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body("Помилка входу");
        }
        String token = jwtUtil.generateToken(userDetails.getUsername());
        return ResponseEntity.ok(token);
    }
}

package com.example.FitPlanner_server.server.Controller;

import com.example.FitPlanner_server.server.DTO.LoginDTO;
import com.example.FitPlanner_server.server.DTO.RegisterDTO;
import com.example.FitPlanner_server.server.Model.UserModel;
import com.example.FitPlanner_server.server.Model.UserRepository;
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

    private UserRepository userRepository;

    public AuthController(JwtUtil jwtUtil, UserRepository userRepository) {
        this.jwtUtil = jwtUtil;
        this.userRepository = userRepository;
    }

    @PostMapping("/auth/register")
    public ResponseEntity<?> register(@RequestBody RegisterDTO registerUser)
    {
        UserModel userModel = userRepository.findByUsername(registerUser.getUsername());
        if(userModel == null)
        {
            UserModel newUser = new UserModel();
            newUser.setName(registerUser.getName());
            newUser.setUsername(registerUser.getUsername());
            newUser.setPassword(passwordEncoder.encode(registerUser.getPassword()));
            userRepository.save(newUser);
            return ResponseEntity.ok("User registered successfully");
        }
        return ResponseEntity.status(HttpStatus.CONFLICT).body("User already exists");
    }

    @PostMapping("/auth/login")
    public ResponseEntity<?> login(@RequestBody LoginDTO loginUser)
    {
        //UserModel userModel = userRepository.findByUsername(loginUser.getUsername());
        Authentication auth = authenticationManager.authenticate
                (
                        new UsernamePasswordAuthenticationToken(loginUser.getUsername(), loginUser.getPassword())
                );
        UserDetails userDetails = (UserDetails) auth.getPrincipal();
        String token = jwtUtil.generateToken(userDetails.getUsername());
        /*if(userModel == null)
        {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body("User not found");
        }
        if(loginUser.getPassword().equals(userModel.getPassword()))
        {
            return ResponseEntity.ok("User logged in successfully");
        }
        return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body("Incorrect password");*/
        return ResponseEntity.ok(token);
    }
}

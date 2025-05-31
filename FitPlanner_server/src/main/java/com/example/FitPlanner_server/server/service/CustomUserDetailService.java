package com.example.FitPlanner_server.server.service;


import com.example.FitPlanner_server.server.Model.UserModel;
import com.example.FitPlanner_server.server.Model.UserRepo;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;

import java.util.Collections;

@Service
public class CustomUserDetailService implements UserDetailsService {

    private UserRepo userRepository;
    public CustomUserDetailService(UserRepo userRepository)
    {
        this.userRepository = userRepository;
    }

    @Override
    public UserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
        UserModel user = userRepository.findByUsername(username);
        if (user == null)
        {
            throw new UsernameNotFoundException("User not found: " + username);
        }
        return new org.springframework.security.core.userdetails.User
                (
                        user.getUsername(),
                        user.getPassword(),
                        Collections.emptyList()
                );
    }
}

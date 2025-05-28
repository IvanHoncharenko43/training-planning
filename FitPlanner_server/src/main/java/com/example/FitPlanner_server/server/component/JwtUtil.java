package com.example.FitPlanner_server.server.component;

import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.SignatureAlgorithm;
import io.jsonwebtoken.security.Keys;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Component;

import javax.crypto.SecretKey;
import java.nio.charset.StandardCharsets;
import java.util.Date;

@Component
public class JwtUtil {

    @Value("${jwt.secret}")
    private String secret;
    @Value("${jwt.expiration}")
    private int jwtExpiration;

    private SecretKey getSecretKey()
    {
        return Keys.hmacShaKeyFor(secret.getBytes(StandardCharsets.UTF_8));
    }

    public String generateToken(String username)
    {
        return Jwts.builder().setSubject(username).setIssuedAt(new Date())
                .setExpiration(new Date(System.currentTimeMillis() + jwtExpiration))
                .signWith(SignatureAlgorithm.HS256, getSecretKey()).compact();
    }
    public String getUsernameFromToken(String token)
    {
        return Jwts.parser().setSigningKey(getSecretKey()).build()
                .parseClaimsJws(token).getBody().getSubject();
    }
    public boolean validateToken(String token)
    {
        try
        {
            Jwts.parser().setSigningKey(getSecretKey()).build().parseClaimsJws(token);
            return true;
        }
        catch (Exception e)
        {
            System.out.println("Invalid JWT");
        }
        return false;
    }
}

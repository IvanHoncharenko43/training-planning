CREATE DATABASE IF NOT EXISTS ${MYSQL_NAME};
USE ${MYSQL_NAME};

CREATE TABLE users
(
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(40),
    username VARCHAR(25) NOT NULL UNIQUE,
    password TEXT
);

CREATE TABLE workouts
(
    id INT PRIMARY KEY AUTO_INCREMENT,
    weight INT,
    notes TEXT,
    hasTrained BOOLEAN,
    userId INT NOT NULL,
    date DATE,
    FOREIGN KEY(userId) REFERENCES users(id)
);
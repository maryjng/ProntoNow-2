CREATE TABLE IF NOT EXISTS users (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    business_id INT,
    email VARCHAR(255) UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (business_id) REFERENCES business(business_id) ON DELETE SET NULL
);

CREATE TABLE IF NOT EXISTS business (
    business_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    address VARCHAR(255) NOT NULL,
    email VARCHAR(255),
    phone_number VARCHAR(15) UNIQUE
);

CREATE TABLE IF NOT EXISTS device (
    device_id INT AUTO_INCREMENT PRIMARY KEY,
    business_id INT,
    make VARCHAR(50) NOT NULL, 
    model VARCHAR(50) NOT NULL, 
    serial_number VARCHAR(100) UNIQUE NOT NULL,
    FOREIGN KEY (business_id) REFERENCES business(business_id) ON DELETE SET NULL
);

CREATE TABLE IF NOT EXISTS report (
    report_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT,
    device_id INT,
    business_id INT,
    error_code VARCHAR(20) NOT NULL, 
    description TEXT NOT NULL,
    severity TINYINT NOT NULL CHECK (severity BETWEEN 1 AND 4),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    attachment_url VARCHAR(500),
    FOREIGN KEY (user_id) REFERENCES user(user_id) ON DELETE SET NULL,
    FOREIGN KEY (device_id) REFERENCES device(device_id) ON DELETE SET NULL,
    FOREIGN KEY (business_id) REFERENCES business(business_id) ON DELETE SET NULL
);
package com.yesblog.domain.core.exceptions;

/**
 * Exception thrown when an invalid password is provided
 */
public class InvalidPasswordException extends DomainException {
    
    public InvalidPasswordException() {
        super("INVALID_PASSWORD", "Invalid password");
    }
}

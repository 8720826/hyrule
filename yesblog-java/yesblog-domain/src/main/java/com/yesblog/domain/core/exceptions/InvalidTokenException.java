package com.yesblog.domain.core.exceptions;

/**
 * Exception thrown when an invalid token is provided
 */
public class InvalidTokenException extends DomainException {
    
    public InvalidTokenException() {
        super("INVALID_TOKEN", "Invalid token");
    }
    
    public InvalidTokenException(String message) {
        super("INVALID_TOKEN", message);
    }
}

package com.yesblog.domain.core.exceptions;

/**
 * Exception thrown when access is denied
 */
public class AccessDeniedException extends DomainException {
    
    public AccessDeniedException() {
        super("ACCESS_DENIED", "Access denied");
    }
    
    public AccessDeniedException(String message) {
        super("ACCESS_DENIED", message);
    }
}

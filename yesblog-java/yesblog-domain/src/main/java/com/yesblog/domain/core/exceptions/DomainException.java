package com.yesblog.domain.core.exceptions;

/**
 * Base exception class for domain exceptions
 */
public class DomainException extends RuntimeException {
    
    private final String code;
    
    public DomainException(String message) {
        super(message);
        this.code = "DOMAIN_ERROR";
    }
    
    public DomainException(String code, String message) {
        super(message);
        this.code = code;
    }
    
    public DomainException(String message, Throwable cause) {
        super(message, cause);
        this.code = "DOMAIN_ERROR";
    }
    
    public String getCode() {
        return code;
    }
}

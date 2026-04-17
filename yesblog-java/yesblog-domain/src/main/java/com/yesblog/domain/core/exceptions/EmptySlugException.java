package com.yesblog.domain.core.exceptions;

/**
 * Exception thrown when a slug is empty
 */
public class EmptySlugException extends DomainException {
    
    public EmptySlugException() {
        super("EMPTY_SLUG", "Slug cannot be empty");
    }
}

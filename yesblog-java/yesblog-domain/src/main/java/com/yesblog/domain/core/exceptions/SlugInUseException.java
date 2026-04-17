package com.yesblog.domain.core.exceptions;

/**
 * Exception thrown when a slug is already in use
 */
public class SlugInUseException extends DomainException {
    
    public SlugInUseException(String slug) {
        super("SLUG_IN_USE", "Slug '" + slug + "' is already in use");
    }
}

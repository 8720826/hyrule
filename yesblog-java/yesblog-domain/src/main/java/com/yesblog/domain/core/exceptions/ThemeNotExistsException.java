package com.yesblog.domain.core.exceptions;

/**
 * Exception thrown when a theme does not exist
 */
public class ThemeNotExistsException extends DomainException {
    
    public ThemeNotExistsException() {
        super("THEME_NOT_EXISTS", "Theme does not exist");
    }
    
    public ThemeNotExistsException(String themeName) {
        super("THEME_NOT_EXISTS", "Theme '" + themeName + "' does not exist");
    }
}

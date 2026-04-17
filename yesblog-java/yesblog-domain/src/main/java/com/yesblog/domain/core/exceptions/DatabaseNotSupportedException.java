package com.yesblog.domain.core.exceptions;

/**
 * Exception thrown when database is not supported
 */
public class DatabaseNotSupportedException extends DomainException {
    
    public DatabaseNotSupportedException(String dbName) {
        super("DATABASE_NOT_SUPPORTED", "Database '" + dbName + "' is not supported");
    }
}

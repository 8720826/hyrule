package com.yesblog.domain.core.exceptions;

/**
 * Exception thrown when file size exceeds limit
 */
public class FileOverflowException extends DomainException {
    
    public FileOverflowException(long maxSize) {
        super("FILE_OVERFLOW", "File size exceeds maximum allowed size of " + maxSize + " bytes");
    }
}

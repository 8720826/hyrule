package com.yesblog.domain.core.exceptions;

/**
 * Exception thrown when file type is not supported
 */
public class FileTypeException extends DomainException {
    
    public FileTypeException(String fileType) {
        super("FILE_TYPE_NOT_SUPPORTED", "File type '" + fileType + "' is not supported");
    }
}

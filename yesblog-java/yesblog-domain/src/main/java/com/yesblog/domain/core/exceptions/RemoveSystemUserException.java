package com.yesblog.domain.core.exceptions;

/**
 * Exception thrown when trying to remove a system user
 */
public class RemoveSystemUserException extends DomainException {
    
    public RemoveSystemUserException() {
        super("REMOVE_SYSTEM_USER_ERROR", "Cannot remove system user");
    }
}

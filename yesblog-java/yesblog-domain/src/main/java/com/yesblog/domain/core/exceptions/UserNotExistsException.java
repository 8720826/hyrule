package com.yesblog.domain.core.exceptions;

/**
 * Exception thrown when a user does not exist
 */
public class UserNotExistsException extends DomainException {
    
    public UserNotExistsException(Integer userId) {
        super("USER_NOT_EXISTS", "User with id " + userId + " does not exist");
    }
}

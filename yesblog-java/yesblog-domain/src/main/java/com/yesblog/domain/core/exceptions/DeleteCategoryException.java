package com.yesblog.domain.core.exceptions;

/**
 * Exception thrown when trying to delete a category that is in use
 */
public class DeleteCategoryException extends DomainException {
    
    public DeleteCategoryException() {
        super("DELETE_CATEGORY_ERROR", "Cannot delete category because it is in use");
    }
}

package com.yesblog.domain.core.exceptions;

/**
 * Exception thrown when a category does not exist
 */
public class CategoryNotExistsException extends DomainException {
    
    public CategoryNotExistsException() {
        super("CATEGORY_NOT_EXISTS", "Category does not exist");
    }
    
    public CategoryNotExistsException(Integer categoryId) {
        super("CATEGORY_NOT_EXISTS", "Category with id " + categoryId + " does not exist");
    }
}

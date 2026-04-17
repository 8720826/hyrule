package com.yesblog.domain.core.exceptions;

/**
 * Exception thrown when a comment does not exist
 */
public class CommentNotExistsException extends DomainException {
    
    public CommentNotExistsException() {
        super("COMMENT_NOT_EXISTS", "Comment does not exist");
    }
    
    public CommentNotExistsException(Integer commentId) {
        super("COMMENT_NOT_EXISTS", "Comment with id " + commentId + " does not exist");
    }
}

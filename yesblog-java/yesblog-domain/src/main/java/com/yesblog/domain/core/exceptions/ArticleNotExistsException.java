package com.yesblog.domain.core.exceptions;

/**
 * Exception thrown when an article does not exist
 */
public class ArticleNotExistsException extends DomainException {
    
    public ArticleNotExistsException() {
        super("ARTICLE_NOT_EXISTS", "Article does not exist");
    }
    
    public ArticleNotExistsException(Integer articleId) {
        super("ARTICLE_NOT_EXISTS", "Article with id " + articleId + " does not exist");
    }
}

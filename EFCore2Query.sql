--select * from Posts;

DELETE P
FROM Posts P
INNER JOIN Blogs B ON P.BlogId = B.BlogId
WHERE P.PostId = 1;
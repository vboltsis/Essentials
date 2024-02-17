/* 10. What are secondary reads in SQL
In SQL, "secondary reads" usually refer to additional reads of data that occur when a query requires data
that is not already present in memory or cached by the database.
These additional reads can slow down query performance,
especially when the data being read is stored on a separate physical disk or in a remote location.

Secondary reads can occur in a number of situations, including:
-When a query joins data from multiple tables, and the necessary data is not already in memory.
-When a query requires data that is not covered by an index, and the data must be read from disk.
-When a query retrieves a large number of rows, and the database needs to access additional data blocks from disk.

To improve query performance and minimize the number of secondary reads,
it is often a good idea to use indexing and other optimization techniques.
This can help ensure that the necessary data is already in memory,
and that the database can retrieve it quickly without needing to perform additional reads. 
 */

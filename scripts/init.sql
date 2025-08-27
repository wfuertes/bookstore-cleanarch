-- Create test database
CREATE DATABASE IF NOT EXISTS bookstore_test;

-- Grant permissions to bookstore_user
GRANT ALL PRIVILEGES ON bookstore.* TO 'bookstore_user'@'%';
GRANT ALL PRIVILEGES ON bookstore_test.* TO 'bookstore_user'@'%';

-- Grant permissions to root for test database
GRANT ALL PRIVILEGES ON bookstore_test.* TO 'root'@'%';

FLUSH PRIVILEGES;

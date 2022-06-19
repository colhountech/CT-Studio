
# Exceptions Strategy

* Better to PreCheck exception than catching exceptions
* Add Prechecks eith via attributes or via MiddleWare routing to detect issues
* Avoid lots of try {} catch (Exceptions){} in code and keep code light.


Did Azure.RequestFailedException happen because of
    
a) eTag out of sync
   or
b) Container does not exist
   or
c) Some connectivity issues
   or
d) maybe it's an OOBE first-time-use and we need to setup dummy data

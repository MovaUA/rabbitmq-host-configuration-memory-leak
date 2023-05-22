# System description

Our production project has the following scenario.

Backend creates jobs for the clients.  
Jobs are registered as an integration messages in RabbitMQ queues.  
Each client has its own durable queue.  
Client connects to the backend via gRPC service endpoint with service stream.  
The gRPC endpoint dynamically "connects" the client to its job queue via MassTransit consumer.  
When gRPC request ends, the endpoint disconnects the connected consumer.  

## Problem description

During the lifetime of the application, a lot of clients connect and disconnect, and this causes the steady growth of memory consumption.

## Repository description

The repository contains a simple application that demonstrates the problem. 
The application project is [MemoryLeakTest](./MemoryLeakTest/MemoryLeakTest.csproj).  
A user of the application can send `connect`/`disconnect` commands to the application via stdin.  
This emulates the clients of our production system which connect and disconnects to/from our backend.  
Over time, the memory consumed by the application growths a lot. Because the following list of endpoints only grows over time:

`this.bus.Topology._hostConfiguration._endpoints`:
- this.bus (MassTransitBus)
    - Topology (RabbitMqBusTopology)
        - _hostConfiguration (RabbitMqHostConfiguration)
            - _endpoints (List<IRabbitMqReceiveEndpointConfiguration>)
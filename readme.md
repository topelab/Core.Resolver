# Topelab Core Resolver

Unified layer resolver to different DI services.

At this first version, Microsoft.Extensions.DependencyInjection and UnityContainer are unified with this layer.

## Changes

### 1.3.722.116

- Added `Add` to ResolverInfoCollection to add others
- Added possibility to add factories to `ResolveInfoCollection` with new extension `AddFactory`.
- Fix `AddResolver` because of different instance of IServiceProvider was using. 

### 1.3.422.113

- Added `AddResolver` to `Topelab.Core.Resolver.Microsoft` to integrate to build services configuration.


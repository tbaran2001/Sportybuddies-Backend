# GraphQL Integration for SportyBuddies API

This document describes the GraphQL implementation added to the SportyBuddies API.

## Overview

GraphQL has been integrated alongside the existing REST API to provide more flexible data fetching capabilities, particularly for the Profiles module.

## Endpoint

GraphQL is available at: `/graphql`

## Features

### 1. Profile Queries
- `getCurrentProfile`: Get the current authenticated user's profile
- `getProfiles`: Get all profiles (currently returns only the current user's profile)
- `getProfileById(profileId: ID!)`: Get a specific profile by ID

### 2. Flexible Data Fetching
GraphQL allows clients to request exactly the data they need. For example:

```graphql
query {
  getCurrentProfile {
    id
    name
    description
    location {
      latitude
      longitude
      address
    }
    sports {
      name
      description
    }
  }
}
```

### 3. Test Query
A simple test query is available:

```graphql
query {
  hello
}
```

## Authentication

Profile queries require authentication. Use JWT tokens in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

## Why GraphQL for Profiles?

The Profiles module was chosen for GraphQL implementation because:

1. **Complex nested data**: ProfileDto contains nested objects (Location, Preferences, Sports)
2. **Flexible querying**: Clients can select exactly which fields they need
3. **Related data**: Profile queries often need related sport and location data
4. **Optimization potential**: GraphQL can optimize data fetching compared to multiple REST calls

## Schema Introspection

GraphQL supports introspection to explore the schema:

```graphql
query {
  __schema {
    types {
      name
      fields {
        name
        type {
          name
        }
      }
    }
  }
}
```

## REST API Compatibility

The existing REST API endpoints remain unchanged and fully functional. GraphQL provides an additional way to access the same data.
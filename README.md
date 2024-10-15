# POC_KeycloackWithDotNet

# KeyCloak Setup
1. Install [Docker](https://www.docker.com/products/docker-desktop/)
1. Pull KeyCloak image from DockerHub with the following command 
`docker pull keycloak/keycloak`
1. Start Keycloak with the following command `docker run -p 8080:8080 -e KC_BOOTSTRAP_ADMIN_USERNAME=admin -e KC_BOOTSTRAP_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak:26.0.0 start-dev` This command starts Keycloak exposed on the local port 8080 and creates an initial admin user with the `username admin` and `password admin`.
1. Log in into [Keycloak](http://localhost:8080/admin)
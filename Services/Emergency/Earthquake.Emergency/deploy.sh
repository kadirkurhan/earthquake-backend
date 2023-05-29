aws ecr get-login-password --region eu-central-1 | docker login --username AWS --password-stdin 168573834994.dkr.ecr.eu-central-1.amazonaws.com

docker buildx build --platform linux/amd64 -t backend-emergency .

docker tag backend-emergency:latest 168573834994.dkr.ecr.eu-central-1.amazonaws.com/backend-emergency:latest

docker push 168573834994.dkr.ecr.eu-central-1.amazonaws.com/backend-emergency:latest

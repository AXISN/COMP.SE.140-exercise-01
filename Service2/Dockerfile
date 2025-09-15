FROM gradle:8.14.3-jdk21 AS build

WORKDIR /app

COPY build.gradle settings.gradle ./
COPY src ./src

RUN gradle build

FROM openjdk:21-jdk AS run

WORKDIR /app

COPY --from=build /app/build/libs/*.jar app.jar

RUN mkdir -p /app/data && chmod 755 /app/data

EXPOSE 5000

CMD ["java", "-jar", "app.jar"]
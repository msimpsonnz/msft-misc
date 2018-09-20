FROM golang:latest as builder
RUN mkdir /build
ADD https://raw.githubusercontent.com/golang/dep/master/install.sh /build
RUN chmod +x /build/install.sh
RUN /build/install.sh
RUN mkdir /go/src/app
ADD . /go/src/app
WORKDIR /go/src/app
RUN dep ensure
RUN CGO_ENABLED=0 GOOS=linux go build -a -installsuffix cgo -ldflags '-extldflags "-static"' -o main .

FROM golang:alpine
COPY --from=builder /go/src/app/main /app/
COPY --from=builder /go/src/app/config.json /app/
WORKDIR /app
EXPOSE 8000/tcp
CMD ["./main"]
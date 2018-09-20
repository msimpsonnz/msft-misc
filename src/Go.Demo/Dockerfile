FROM golang:latest as builder
RUN mkdir /build
ADD https://raw.githubusercontent.com/golang/dep/master/install.sh /build
RUN chmod +x /build/install.sh
RUN /build/install.sh
RUN mkdir /go/src/app
ADD . /go/src/app
WORKDIR /go/src/app
RUN dep ensure
RUN CGO_ENABLED=0 GOOS=linux go build -a -installsuffix cgo -o main .

FROM alpine:latest as alpine
RUN apk --no-cache add ca-certificates

FROM scratch
COPY --from=builder /go/src/app/main /app/
COPY --from=builder /go/src/app/config.json /app/
COPY --from=alpine /etc/ssl/certs/ca-certificates.crt /etc/ssl/certs/
WORKDIR /app
EXPOSE 8000/tcp
CMD ["./main"]
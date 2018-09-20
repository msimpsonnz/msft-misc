FROM golang:latest as builder
RUN mkdir /build
ADD https://raw.githubusercontent.com/golang/dep/master/install.sh /build
RUN chmod +x ./install.sh
RUN ./install.sh
ADD . /build/
WORKDIR /build
RUN dep ensure
RUN CGO_ENABLED=0 GOOS=linux go build -a -installsuffix cgo -ldflags '-extldflags "-static"' -o main .

FROM scratch
COPY --from=builder /build/main /app/
WORKDIR /app
CMD ["./main"]
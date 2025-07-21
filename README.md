## Overview
.NET console application that takes in URL as an input and crawls the website. For each page, the console prints the page's URL and all the urls found on this page.

### Architecture
This is a sketch of the architecture diagram that was used to implement the service's dependencies.
![Architecture of web crawler.](/Documentation/architecture-diagram.png)

## How to run
- Install .NET 8 SDK to run the app
- Open terminal and enter repositories main directory
- Enter `/WebCrawler` directory: `cd WebCrawler`
- Run `dotnet run <uri-of-page-to-crawl>`

## Notes
### Tools used
- draw.io for diagram
- Rider for service implementation
- Microsoft .NET documentation website
- ChatGPT for looking up .NET documentation
- Copilot to assist wih sketching the integration tests
- VS Code for ReadME edit

### Considerations
- Crawler was tested on the `https://crawler-test.com/`
- The service was designed with the assumption that it should be easily divided into seperate microservices and enable horizontal scaling.
- The entrypoint to he service could have been an API or a queue, where each request/message could be containing a request to crawl a particular domain.
- The crawler currenlty supports only the relative links starting with root `/`. In the future, service should be able to handle:
    - relative uri without root `/`
    - full website links that are in the domain
    - filtering out fragments `#`
    - filtering out links to javascript and other non html documents
- Service should have more behaviour tests to check the edge cases mentioned in the bullet point above.
- Crawler should read the robot.txt file
- Service could use better implementation of Http request handling. Currently the app does not utilize the connection pool efficiently. Also it lacks any resilience policy except the default timeouts.
- As it would take some effort, I skipped setting up proper integration tests. I started writing the example of the integration test but came to the conclusion that I would need to replace the HtmlDoc dependency so I could make a decent setup for the tests.
- repository lacks continuous integration pipeline e.g github actions setup.
- Naming could have been better, with the proper discovery of the domain, the structure of the project would have made more sense.


## Backend for a-shitty-off-brand-snake-game

Build with Serverless, hosted on AWS lambda.
MongoDB hosted at mlab.com.

## How to crank it up

1. npm i
2. create config.js in backend/ defining mongodURI
3. sls offline

### config.js example

```js
module.exports = {
  mongoURI: "mongodb://<username>:<password>.mlab.com",
};
```

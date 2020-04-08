const response = {
  defaultHeaders: {
    "Access-Control-Allow-Origin": "*",
    "Access-Control-Allow-Credentials": true,
  },
  buildResponse: function (body, statusCode, headers) {
    let res = {
      statusCode: statusCode,
      headers: Object.assign(
        {},
        {
          "Access-Control-Allow-Origin": "*",
          "Access-Control-Allow-Credentials": true,
        },
        headers
      ),
    };
    if (body !== null) {
      res.body = JSON.stringify(body);
    }
    return res;
  },
  success: function (body, statusCode = 200, headers = {}) {
    return response.buildResponse(body, statusCode, headers);
  },
  failure: function (body, statusCode = 500, headers = {}) {
    return response.buildResponse(body, statusCode, headers);
  },
  Response: (callback) => ({
    send: (body, status, headers) =>
      callback(null, response.success(body, status, headers)),
    error: (body, status, headers) =>
      callback(null, response.failure(body, status, headers)),
    redirect: (url, status = 303) =>
      callback(null, response.success(undefined, status, { location: url })),
    notAuthenticated: () =>
      callback(null, response.failure({ message: "Not Authenticated" }, 401)),
  }),
};
module.exports = response;

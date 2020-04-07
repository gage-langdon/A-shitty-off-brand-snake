const uuid = require("uuid/v4");

module.exports.getScores = async (event) => {
  return {
    statusCode: 200,
    body: JSON.stringify(
      {
        scores: [{ id: uuid(), name: "FLEEB", score: 1 }],
      },
      null,
      2
    ),
  };
};

module.exports.postScore = async (event) => {
  return {
    statusCode: 200,
    body: JSON.stringify(
      {
        score: { id: uuid(), name: "FLEEB", score: 1 },
      },
      null,
      2
    ),
  };
};

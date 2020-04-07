const uuid = require("uuid/v4");
const { dynamoDb } = require("../utils/dynamo");

const TableName = process.env.SCORES_TABLE;

module.exports.getScores = async () => {
  //   const params = {
  //     TableName,
  //     KeyConditionExpression: 'id = :id',
  //     ExpressionAttributeValues: { ':pub_id': '700'}
  //   };

  //   const { items = [] } = (await dynamoDb.query(params).promise()) || {};

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
  try {
    const { score, name } = JSON.parse(event.body);

    const Item = {
      id: uuid(),
      score,
      name,
    };

    const params = {
      TableName,
      Item,
    };

    await dynamoDb.put(params).promise();

    return {
      statusCode: 200,
      body: JSON.stringify(Item, null, 2),
    };
  } catch (e) {
    console.log("Failed to post score. Error: ", e);

    return {
      statusCode: 400,
      body: JSON.stringify({ message: "Failed to post score." }, null, 2),
    };
  }
};

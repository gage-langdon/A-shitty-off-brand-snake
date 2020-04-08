const mongoose = require("mongoose");
const Schema = mongoose.Schema;
const ObjectId = Schema.ObjectId;
const mongo = require("../services/mongodb");

const User = mongoose.model(
  "User",
  new Schema({
    id: ObjectId,
    score: Number,
    name: String,
    dateCreated: Date,
  })
);

module.exports = async () => {
  await mongo.connect();
  return {
    create: ({ name, score }) =>
      User.create({
        name,
        score,
        dateCreated: Date.now(),
      }),
    getById: (id) => User.findById(id),
    getUsersSortedByScore: ({ limit = 10 }) =>
      User.find().limit(limit).sort("-score"),
  };
};

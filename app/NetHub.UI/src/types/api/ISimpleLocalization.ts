﻿import { ContentStatus, IArticleContributorModel, Vote } from "../../api/_api";
import { DateTime } from "luxon";

export interface ISimpleLocalization {
  id: number
  articleId: number;
  languageCode: string;
  title: string;
  description: string;
  status: ContentStatus;
  views: number;
  rate: number;
  created: DateTime;
  updated: DateTime | null;
  published: DateTime | null;
  banned: DateTime | null;
  isSaved: boolean;
  savedDate: DateTime | null;
  vote: Vote | null;
}
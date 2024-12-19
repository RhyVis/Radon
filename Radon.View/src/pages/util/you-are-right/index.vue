<script lang="ts" setup>
import { get } from "@vueuse/core";
import { computed, ref } from "vue";
import { useI18n } from "vue-i18n";

const { t } = useI18n();
const original =
  "你说的对，但是《原神》是由米哈游自主研发的一款全新开放世界冒险游戏。" +
  "游戏发生在一个被称作「提瓦特」的幻想世界，在这里，被神选中的人将被授予「神之眼」，导引元素之力。" +
  "你将扮演一位名为「旅行者」的神秘角色，在自由的旅行中邂逅性格各异、能力独特的同伴们，和他们一起击败强敌，找回失散的亲人——" +
  "同时，逐步发掘「原神」的真相。";

const start = ref(false);
const gTitle = ref("原神");
const gCompany = ref("米哈游");
const gWorld = ref("提瓦特");
const gGod = ref("神");
const gGodEye = ref("神之眼");
const gPower = ref("元素之力");
const gTraveler = ref("旅行者");
const gTravel = ref("旅行");
const gFriends = ref("同伴");
const gEnemies = ref("强敌");
const gFamily = ref("亲人");
const gTruth = ref("原神");

const youAreRight = computed(
  () =>
    `你说的对，但是《${get(gTitle)}》是由${get(gCompany)}自主研发的一款全新开放世界冒险游戏。` +
    `游戏发生在一个被称作「${get(gWorld)}」的幻想世界，在这里，被${get(gGod)}选中的人将被授予「${get(gGodEye)}」，导引${get(gPower)}。` +
    `你将扮演一位名为「${get(gTraveler)}」的神秘角色，在自由的${get(gTravel)}中邂逅性格各异、能力独特的${get(gFriends)}们，和他们一起击败${get(gEnemies)}，找回失散的${get(gFamily)}——` +
    `同时，逐步发掘「${get(gTruth)}」的真相。`,
);
</script>

<template>
  <content-layout :subtitle="t('st')" :title="t('tt')">
    <div v-if="start">
      <t-card>
        <t-text>{{ youAreRight }}</t-text>
      </t-card>
      <div class="mt-4">
        <t-card>
          <t-form>
            <t-row :gutter="[6, 6]">
              <t-col :sm="4" :xs="6">
                <t-input v-model="gTitle" label="Title" placeholder="原神" />
              </t-col>
              <t-col :sm="4" :xs="6">
                <t-input v-model="gCompany" label="Company" placeholder="米哈游" />
              </t-col>
              <t-col :sm="4" :xs="6">
                <t-input v-model="gWorld" label="World" placeholder="提瓦特" />
              </t-col>
              <t-col :sm="4" :xs="6">
                <t-input v-model="gGod" label="God" placeholder="神" />
              </t-col>
              <t-col :sm="4" :xs="6">
                <t-input v-model="gGodEye" label="God Eye" placeholder="神之眼" />
              </t-col>
              <t-col :sm="4" :xs="6">
                <t-input v-model="gPower" label="Power" placeholder="元素之力" />
              </t-col>
              <t-col :sm="4" :xs="6">
                <t-input v-model="gTraveler" label="Traveler" placeholder="旅行者" />
              </t-col>
              <t-col :sm="4" :xs="6">
                <t-input v-model="gTravel" label="Travel" placeholder="旅行" />
              </t-col>
              <t-col :sm="4" :xs="6">
                <t-input v-model="gFriends" label="Friends" placeholder="同伴" />
              </t-col>
              <t-col :sm="4" :xs="6">
                <t-input v-model="gEnemies" label="Enemies" placeholder="强敌" />
              </t-col>
              <t-col :sm="4" :xs="6">
                <t-input v-model="gFamily" label="Family" placeholder="亲人" />
              </t-col>
              <t-col :sm="4" :xs="6">
                <t-input v-model="gTruth" label="Truth" placeholder="原神" />
              </t-col>
            </t-row>
          </t-form>
          <template #footer>
            <BtnCopy :value="youAreRight" />
          </template>
        </t-card>
      </div>
    </div>
    <div v-else>
      <t-card>
        <t-text>{{ original }}</t-text>
        <template #footer>
          <div class="r-no-select text-center font-medium" @click="start = true">[ {{ t("clickToEdit") }} ]</div>
        </template>
      </t-card>
    </div>
  </content-layout>
</template>

<i18n locale="en">
tt: You're right
st: But...
clickToEdit: Click to edit
</i18n>

<i18n locale="zh-CN">
tt: 你说得对
st: 但是...
clickToEdit: 点击进行编辑
</i18n>

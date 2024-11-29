<script setup lang="ts">
import { intToRoman } from "@/pages/math/roman/scripts/romanNum";
import { computed, ref } from "vue";

const input = ref(0);
const roman = computed(() => intToRoman(input.value));
</script>

<template>
  <content-layout title="罗马数字" subtitle="古典计数法">
    <t-form>
      <t-form-item label="阿拉伯数字">
        <t-input v-model="input" type="number" />
      </t-form-item>
      <t-form-item label="罗马数字">
        <t-textarea :value="roman" :readonly="true" :autosize="true" />
      </t-form-item>
      <t-form-item label="操作">
        <t-space :size="5">
          <btn-copy :value="roman" />
        </t-space>
      </t-form-item>
    </t-form>
  </content-layout>
</template>

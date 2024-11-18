<script setup lang="ts">
import { basicCharset, extendedCharset, radixVal } from "@/pages/math/radix/scripts/radix";
import { ArrowRightCircleIcon, ChartRingIcon, RefreshIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { computed, reactive, ref } from "vue";
import { set } from "@vueuse/core";

const defaultCharset = basicCharset;
const query = reactive({
  input: "10",
  iRadix: 10,
  oRadix: 10,
  iCharset: basicCharset,
  oCharset: basicCharset,
});
const iCharsetInputStatus = ref<"default" | "error">("default");
const oCharsetInputStatus = ref<"default" | "error">("default");
const result = computed(() => radixVal(query.input, query.iRadix, query.oRadix, query.iCharset, query.oCharset));
const iOpt = computed(() => generateOpt(query.iCharset));
const oOpt = computed(() => generateOpt(query.oCharset));
const generateOpt = (charset: string) => {
  return Array.from({ length: charset.length + 1 }, (_, i) => i)
    .slice(2)
    .map(i => ({ label: i.toString(), value: i }));
};

const handleCharsetChange = () => {
  set(iCharsetInputStatus, "default");
  set(oCharsetInputStatus, "default");
  const iL = query.iCharset.length;
  const oL = query.oCharset.length;
  if (query.iRadix > iL && iL > 2) {
    query.iRadix = iL;
  }
  if (query.oRadix > oL && oL > 2) {
    query.oRadix = oL;
  }
  if (iL < 2) {
    MessagePlugin.warning("字符集长度不能小于2");
    set(iCharsetInputStatus, "error");
  }
  if (oL < 2) {
    MessagePlugin.warning("字符集长度不能小于2");
    set(oCharsetInputStatus, "error");
  }
  if (new Set(query.iCharset).size < iL) {
    MessagePlugin.warning("字符集不允许重复字符");
    set(iCharsetInputStatus, "error");
  }
  if (new Set(query.oCharset).size < oL) {
    MessagePlugin.warning("字符集不允许重复字符");
    set(oCharsetInputStatus, "error");
  }
};
const handleCharsetInputReset = () => {
  query.iCharset = defaultCharset;
  query.iRadix = 10;
};
const handleCharsetOutputReset = () => {
  query.oCharset = defaultCharset;
  query.oRadix = 10;
};
const handleExtended = () => {
  query.iCharset = extendedCharset;
  query.oCharset = extendedCharset;
};
</script>

<template>
  <content-layout title="进制转换" subtitle="注意输入内容符合字符集">
    <t-form label-align="top">
      <t-form-item label="数字">
        <t-space align="center">
          <t-input class="r-input-text-align-for-num" v-model="query.input" />
          <ArrowRightCircleIcon />
          <t-input class="r-input-text-align-for-num" :value="result" placeholder="输入值来转换" :readonly="true" />
        </t-space>
      </t-form-item>
      <t-form-item label="进制">
        <t-space align="center">
          <t-select class="r-input-text-align-for-num" v-model="query.iRadix" :options="iOpt" :filterable="true" />
          <ArrowRightCircleIcon />
          <t-select class="r-input-text-align-for-num" v-model="query.oRadix" :options="oOpt" :filterable="true" />
        </t-space>
      </t-form-item>
      <t-form-item label="高级">
        <adv-opt-container>
          <t-space :size="4" direction="vertical" align="center">
            <t-space :size="4">
              <t-tooltip placement="top" content="按顺序定义基数字符，不允许重复">
                <t-tag class="r-no-select" content="输入字符集" />
              </t-tooltip>
              <t-button size="small" theme="default" @click="handleCharsetInputReset">
                <RefreshIcon />
              </t-button>
            </t-space>
            <t-textarea
              v-model="query.iCharset"
              :status="iCharsetInputStatus"
              :autosize="true"
              @change="handleCharsetChange"
            />
            <t-space :size="4">
              <t-tooltip placement="top" content="按顺序定义基数字符，不允许重复">
                <t-tag class="r-no-select" content="输出字符集" />
              </t-tooltip>
              <t-button size="small" theme="default" @click="handleCharsetOutputReset">
                <RefreshIcon />
              </t-button>
            </t-space>
            <t-textarea
              v-model="query.oCharset"
              :status="oCharsetInputStatus"
              :autosize="true"
              @change="handleCharsetChange"
            />
            <t-space :size="4">
              <t-tag class="r-no-select" content="抽象版本" />
              <t-button size="small" theme="default" @click="handleExtended">
                <ChartRingIcon />
              </t-button>
            </t-space>
          </t-space>
        </adv-opt-container>
      </t-form-item>
    </t-form>
  </content-layout>
</template>

<style scoped>
.r-input-text-align-for-num {
  width: 144px;
}
</style>
